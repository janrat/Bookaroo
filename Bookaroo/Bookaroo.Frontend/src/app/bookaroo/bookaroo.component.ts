import { Component, NgModule, OnInit } from '@angular/core';
import { NgForm, FormsModule } from '@angular/forms';
import { CommonModule, DatePipe } from '@angular/common';
import { ServicesComponent } from '../services/services.component';
import { BookDto, PublisherDto, CategoryDto, AuthorDto } from './bookaroo.model';
import { CreateBookDto, UpdateBookDto, CreateAuthorDto, UpdateAuthorDto, CreateCategoryDto, UpdateCategoryDto, CreatePublisherDto, UpdatePublisherDto } from './bookaroo.model';
import { AuthService } from '../auth/auth.service';
import { Router } from '@angular/router';
import { RoleService } from '../auth/role.service';
import { TranslatePipe } from "../translate/translate.pipe";

declare var bootstrap: any;

@Component({
  selector: 'app-bookaroo',
  standalone: true,
  templateUrl: './bookaroo.component.html',
  styleUrls: ['./bookaroo.component.scss'],
  providers: [DatePipe],
  imports: [FormsModule, CommonModule, TranslatePipe]
})
export class BookarooComponent implements OnInit {
  books: BookDto[] = [];
  authors: AuthorDto[] = [];
  categories: CategoryDto[] = [];
  publishers: PublisherDto[] = [];
  searchTerm: string = '';
  page: number = 1;
  pageSize: number = 10;
  totalPages: number = 1;
  userRole: string | null = null;
  newBookPublicationDate: string | null = null;
  authorIdsInput: string = '';

  // Book Modal
  newBook: CreateBookDto | UpdateBookDto = {} as CreateBookDto;
  selectedBook: BookDto | null = null;

  // Author Modal
  newAuthor: CreateAuthorDto | UpdateAuthorDto = {} as CreateAuthorDto;
  selectedAuthor: AuthorDto | null = null;

  // Category Modal
  newCategory: CreateCategoryDto | UpdateCategoryDto = {} as CreateCategoryDto;
  selectedCategory: CategoryDto | null = null;

  // Publisher Modal
  newPublisher: CreatePublisherDto | UpdatePublisherDto = {} as CreatePublisherDto;
  selectedPublisher: PublisherDto | null = null;

  constructor(
    private datePipe: DatePipe,
    private servicesComponent: ServicesComponent,
    private authService: AuthService,
    private router: Router,
    private roleService: RoleService
  ) { }

  ngOnInit() {
    const savedRole = this.roleService.getRole();
    if (savedRole) {
      this.userRole = savedRole;
    } else {
      this.router.navigate(['/login']);
    }
    this.getAllBooks();
  }

  logout() {
    this.authService.logout();
    this.router.navigate(['/login']);
  }

  previousPage() {
    if (this.page > 1) {
      this.page--;
      this.getAllBooks();
    }
  }

  nextPage() {
    if (this.page < this.totalPages) {
      this.page++;
      this.getAllBooks();
    }
  }

  updateAuthorIds(): void {
    this.newBook.authorIds = this.authorIdsInput
      .split(',')
      .map(id => parseInt(id.trim(), 10)) // Convert to integers
      .filter(id => !isNaN(id)); // Remove invalid numbers
  }

  // ===================== Books =====================
  getAllBooks() {
    this.servicesComponent.getAllBooks(this.page, this.pageSize, this.searchTerm).subscribe((data) => {
      this.books = data.items;
      this.totalPages = data.totalPages;

      this.books.forEach((book) => {
        this.loadPublisher(book);
        this.loadCategory(book);
        this.loadAuthors(book);
      });
    },
      (error) => {
        console.error('Error fetching books:', error);
      });
  }

  loadPublisher(book: BookDto) {
    if (!book.publisherId) return;
    this.servicesComponent.getPublisherById(book.publisherId).subscribe(
      (publisher) => {
        book.publisher = publisher;
      },
      (error) => {
        console.error(`Error fetching publisher for book ${book.bookId}:`, error);
      }
    );
  }

  loadCategory(book: BookDto) {
    if (!book.categoryId) return;
    this.servicesComponent.getCategoryById(book.categoryId).subscribe(
      (category) => {
        book.category = category;
      },
      (error) => {
        console.error(`Error fetching category for book ${book.bookId}:`, error);
      }
    );
  }

  loadAuthors(book: BookDto) {
    if (!book.authorIds || book.authorIds.length === 0) return;
    book.authors = []; // Initialize authors array

    book.authorIds.forEach((authorId) => {
      this.servicesComponent.getAuthorById(authorId).subscribe(
        (author) => {
          book.authors.push(author);
        },
        (error) => {
          console.error(`Error fetching author ${authorId} for book ${book.bookId}:`, error);
        }
      );
    });
  }

  openBookModal(book: BookDto | null) {
    if (book) {
      this.selectedBook = book;
      this.newBook = {
        title: book.title || '',
        isbn: book.isbn || '',
        publicationDate: book.publicationDate || null,
        pages: book.pages || null,
        price: book.price || null,
        publisherId: book.publisherId || 0,
        categoryId: book.categoryId || 0,
        authorIds: book.authorIds || []
      };
      this.newBookPublicationDate = this.datePipe.transform(book.publicationDate, 'yyyy-MM-dd');
      this.authorIdsInput = (this.newBook.authorIds || []).join(', ');
    } else {
      this.selectedBook = null;
      this.newBook = {
        title: '',
        isbn: '',
        publicationDate: null,
        pages: null,
        price: null,
        publisherId: 0,
        categoryId: 0,
        authorIds: []
      } as CreateBookDto;
      this.newBookPublicationDate = null;
      this.authorIdsInput = null;
    }

    const modalElement = document.getElementById('bookModal');
    if (modalElement) {
      const modal = new bootstrap.Modal(modalElement);
      modal.show();
    }
  }

  saveBook(form: NgForm) {
    if (form.invalid) {
      alert('Form is invalid. Please fill out all required fields.');
      return;
    }

    const bookDto = this.selectedBook ? this.createUpdateBookDto() : this.createNewBookDto();

    console.log('Payload being sent:', bookDto); // Log payload for debugging

    if (this.selectedBook) {
      this.servicesComponent.updateBook(this.selectedBook.bookId, bookDto).subscribe(
        () => {
          this.getAllBooks();
        },
        (error) => {
          console.error('Error updating book:', error);
          alert('Failed to update book.');
        }
      );
    } else {
      this.servicesComponent.createBook(bookDto).subscribe(
        () => {
          this.getAllBooks();
        },
        (error) => {
          console.error('Error creating book:', error);
          alert('Failed to create book.');
        }
      );
    }

    const modalElement = document.getElementById('bookModal');
    if (modalElement) {
      const modal = bootstrap.Modal.getInstance(modalElement);
      modal.hide();
    }
  }


  createUpdateBookDto(): UpdateBookDto {
    return {
      title: this.newBook.title?.trim() || '',
      isbn: this.newBook.isbn?.trim() || '',
      publicationDate: this.newBookPublicationDate ? new Date(this.newBookPublicationDate) : null,
      pages: this.newBook.pages || null,
      price: this.newBook.price || null,
      publisherId: this.newBook.publisherId || 0,
      categoryId: this.newBook.categoryId || 0,
      authorIds: this.newBook.authorIds || []
    };
  }

  createNewBookDto(): CreateBookDto {
    return {
      title: this.newBook.title?.trim() || '',
      isbn: this.newBook.isbn?.trim() || '',
      publicationDate: this.newBookPublicationDate ? new Date(this.newBookPublicationDate) : null,
      pages: this.newBook.pages || null,
      price: this.newBook.price || null,
      publisherId: this.newBook.publisherId || 0,
      categoryId: this.newBook.categoryId || 0,
      authorIds: this.newBook.authorIds || []
    };
  }


  deleteBook(book: BookDto) {
    if (this.userRole !== 'Admin') {
      alert('You do not have permission to delete books.');
      return;
    }
    this.servicesComponent.deleteBook(book.bookId).subscribe(() => {
      this.getAllBooks();
    });
  }

  // ===================== Authors =====================

  openAuthorModal(author: AuthorDto | null) {
    if (author) {
      this.selectedAuthor = author;
      this.newAuthor = {
        firstName: author.firstName,
        lastName: author.lastName,
        dateOfBirth: author.dateOfBirth,
        bio: author.bio
      };
    } else {
      this.selectedAuthor = null;
      this.newAuthor = {} as CreateAuthorDto;
    }
    const modalElement = document.getElementById('authorModal');
    if (modalElement) {
      const modal = new bootstrap.Modal(modalElement);
      modal.show();
    }
  }

  saveAuthor(form: NgForm) {
    if (form.invalid) {
      return;
    }
    if (this.selectedAuthor) {
      const updatedAuthor: UpdateAuthorDto = { ...this.newAuthor };
      this.servicesComponent.updateAuthor(this.selectedAuthor.authorId, updatedAuthor).subscribe(() => {
        this.getAllBooks();
      });
    } else {
      const newAuthor: CreateAuthorDto = { ...this.newAuthor };
      this.servicesComponent.createAuthor(newAuthor).subscribe(() => {
        this.getAllBooks();
      });
    }
    const modalElement = document.getElementById('authorModal');
    if (modalElement) {
      const modal = bootstrap.Modal.getInstance(modalElement);
      modal.hide();
    }
  }

  deleteAuthor(author: AuthorDto) {
    this.servicesComponent.deleteAuthor(author.authorId).subscribe(() => {
      this.getAllBooks();
    });
  }

  // ===================== Categories =====================

  openCategoryModal(category: CategoryDto | null) {
    if (category) {
      this.selectedCategory = category;
      this.newCategory = { name: category.name };
    } else {
      this.selectedCategory = null;
      this.newCategory = {} as CreateCategoryDto;
    }
    const modalElement = document.getElementById('categoryModal');
    if (modalElement) {
      const modal = new bootstrap.Modal(modalElement);
      modal.show();
    }
  }

  saveCategory(form: NgForm) {
    if (form.invalid) {
      return;
    }
    if (this.selectedCategory) {
      const updatedCategory: UpdateCategoryDto = { ...this.newCategory };
      this.servicesComponent.updateCategory(this.selectedCategory.categoryId, updatedCategory).subscribe(() => {
        this.getAllBooks();
      });
    } else {
      const newCategory: CreateCategoryDto = { ...this.newCategory };
      this.servicesComponent.createCategory(newCategory).subscribe(() => {
        this.getAllBooks();
      });
    }
    const modalElement = document.getElementById('categoryModal');
    if (modalElement) {
      const modal = bootstrap.Modal.getInstance(modalElement);
      modal.hide();
    }
  }

  deleteCategory(category: CategoryDto) {
    this.servicesComponent.deleteCategory(category.categoryId).subscribe(() => {
      this.getAllBooks();
    });
  }

  // ===================== Publishers =====================

  openPublisherModal(publisher: PublisherDto | null) {
    if (publisher) {
      this.selectedPublisher = publisher;
      this.newPublisher = { name: publisher.name, address: publisher.address };
    } else {
      this.selectedPublisher = null;
      this.newPublisher = {} as CreatePublisherDto;
    }
    const modalElement = document.getElementById('publisherModal');
    if (modalElement) {
      const modal = new bootstrap.Modal(modalElement);
      modal.show();
    }
  }

  savePublisher(form: NgForm) {
    if (form.invalid) {
      return;
    }
    if (this.selectedPublisher) {
      const updatedPublisher: UpdatePublisherDto = { ...this.newPublisher };
      this.servicesComponent.updatePublisher(this.selectedPublisher.publisherId, updatedPublisher).subscribe(() => {
        this.getAllBooks();
      });
    } else {
      const newPublisher: CreatePublisherDto = { ...this.newPublisher };
      this.servicesComponent.createPublisher(newPublisher).subscribe(() => {
        this.getAllBooks();
      });
    }
    const modalElement = document.getElementById('publisherModal');
    if (modalElement) {
      const modal = bootstrap.Modal.getInstance(modalElement);
      modal.hide();
    }
  }

  deletePublisher(publisher: PublisherDto) {
    this.servicesComponent.deletePublisher(publisher.publisherId).subscribe(() => {
      this.getAllBooks();
    });
  }
}
