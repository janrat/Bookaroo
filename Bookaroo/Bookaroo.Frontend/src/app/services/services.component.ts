import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { BookDto, AuthorDto, CategoryDto, PublisherDto } from '../bookaroo/bookaroo.model';
import { CreateBookDto, CreateAuthorDto, CreateCategoryDto, CreatePublisherDto } from '../bookaroo/bookaroo.model';
import { UpdateBookDto, UpdateAuthorDto, UpdateCategoryDto, UpdatePublisherDto } from '../bookaroo/bookaroo.model';
import { backendurl } from '../services/backendurl';

@Injectable({
  providedIn: 'root'
})
export class ServicesComponent {
  private baseUrl = backendurl.urlString;

  constructor(private http: HttpClient) {}

  // ===================== Books =====================
  getAllBooks(pageNumber: number = 1, pageSize: number = 10, search: string = ''): Observable<any> {
    const token = localStorage.getItem('token');
    const headers = token ? { Authorization: `Bearer ${token}` } : {};
    let params = new HttpParams()
      .set('pageNumber', pageNumber.toString())
      .set('pageSize', pageSize.toString());

    if (search) {
      params = params.set('search', search);
    }
    return this.http.get<any>(`${this.baseUrl}/Book`, { headers, params });
  }

  getBookById(id: number): Observable<BookDto> {
    const token = localStorage.getItem('token');
    const headers = token ? { Authorization: `Bearer ${token}` } : {};
    return this.http.get<BookDto>(`${this.baseUrl}/Book/${id}`, { headers });
  }

  createBook(book: CreateBookDto): Observable<CreateBookDto> {
    const token = localStorage.getItem('token');
    const headers = token ? { Authorization: `Bearer ${token}` } : {};
    return this.http.post<CreateBookDto>(`${this.baseUrl}/Book`, book, { headers });
  }

  updateBook(id: number, book: UpdateBookDto): Observable<void> {
    const token = localStorage.getItem('token');
    const headers = token ? { Authorization: `Bearer ${token}` } : {};
    return this.http.put<void>(`${this.baseUrl}/Book/${id}`, book, { headers });
  }

  deleteBook(id: number): Observable<void> {
    const token = localStorage.getItem('token');
    const headers = token ? { Authorization: `Bearer ${token}` } : {};
    return this.http.delete<void>(`${this.baseUrl}/Book/${id}`, { headers });
  }

  // ===================== Authors =====================
  getAllAuthors(pageNumber: number = 1, pageSize: number = 10, search: string = ''): Observable<any> {
    const params = new HttpParams()
      .set('pageNumber', pageNumber.toString())
      .set('pageSize', pageSize.toString())
      .set('search', search);
    return this.http.get(`${this.baseUrl}/Author`, { params });
  }

  getAuthorById(id: number): Observable<AuthorDto> {
    return this.http.get<AuthorDto>(`${this.baseUrl}/Author/${id}`);
  }

  createAuthor(author: CreateAuthorDto): Observable<CreateAuthorDto> {
    return this.http.post<CreateAuthorDto>(`${this.baseUrl}/Author`, author);
  }

  updateAuthor(id: number, author: UpdateAuthorDto): Observable<void> {
    return this.http.put<void>(`${this.baseUrl}/Author/${id}`, author);
  }

  deleteAuthor(id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/Author/${id}`);
  }

  // ===================== Categories =====================
  getAllCategories(pageNumber: number = 1, pageSize: number = 10, search: string = ''): Observable<any> {
    const params = new HttpParams()
      .set('pageNumber', pageNumber.toString())
      .set('pageSize', pageSize.toString())
      .set('search', search);
    return this.http.get(`${this.baseUrl}/Category`, { params });
  }

  getCategoryById(id: number): Observable<CategoryDto> {
    return this.http.get<CategoryDto>(`${this.baseUrl}/Category/${id}`);
  }

  createCategory(category: CreateCategoryDto): Observable<CreateCategoryDto> {
    return this.http.post<CreateCategoryDto>(`${this.baseUrl}/Category`, category);
  }

  updateCategory(id: number, category: UpdateCategoryDto): Observable<void> {
    return this.http.put<void>(`${this.baseUrl}/Category/${id}`, category);
  }

  deleteCategory(id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/Category/${id}`);
  }

  // ===================== Publishers =====================
  getAllPublishers(pageNumber: number = 1, pageSize: number = 10, search: string = ''): Observable<any> {
    const params = new HttpParams()
      .set('pageNumber', pageNumber.toString())
      .set('pageSize', pageSize.toString())
      .set('search', search);
    return this.http.get(`${this.baseUrl}/Publisher`, { params });
  }

  getPublisherById(id: number): Observable<PublisherDto> {
    return this.http.get<PublisherDto>(`${this.baseUrl}/Publisher/${id}`);
  }

  createPublisher(publisher: CreatePublisherDto): Observable<CreatePublisherDto> {
    return this.http.post<CreatePublisherDto>(`${this.baseUrl}/Publisher`, publisher);
  }

  updatePublisher(id: number, publisher: UpdatePublisherDto): Observable<void> {
    return this.http.put<void>(`${this.baseUrl}/Publisher/${id}`, publisher);
  }

  deletePublisher(id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/Publisher/${id}`);
  }
}
