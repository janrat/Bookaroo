// ===================== Read =====================
export class BookDto {
  bookId: number;
  title: string;
  isbn: string;
  publicationDate?: Date | null;
  pages?: number | null;
  price?: number | null;
  publisherId: number | null;
  categoryId: number | null;
  authorIds: number[] | null;
  publisher: PublisherDto;
  category: CategoryDto;
  authors: AuthorDto[];
}

export class AuthorDto {
  authorId: number;
  firstName: string;
  lastName: string;
  dateOfBirth?: Date | null;
  bio?: string | null;
  bookTitles?: string[];
  books?: BookDto[];
}
export class PublisherDto {
  publisherId: number;
  name: string;
  address: string;
  bookTitles?: string[];
  books?: BookDto[];
}
export class CategoryDto {
  categoryId: number;
  name: string;
  bookTitles?: string[];
  books?: BookDto[];
}

// ===================== Create =====================
export class CreateBookDto {
  title: string;
  isbn: string;
  publicationDate?: Date | null;
  pages?: number | null;
  price?: number | null;
  publisherId: number;
  categoryId: number;
  authorIds: number[];
}

export class CreateAuthorDto {
  firstName: string;
  lastName: string;
  dateOfBirth?: Date | null;
  bio?: string | null;
}

export class CreatePublisherDto {
  name: string;
  address: string;
}

export class CreateCategoryDto {
  name: string;
}

// ===================== Update =====================
export class UpdateBookDto {
  title: string = 'asd'; 
  isbn: string;
  publicationDate?: Date | null;
  pages?: number | null;
  price?: number | null;
  publisherId: number;
  categoryId: number;
  authorIds: number[];
}

export class UpdateAuthorDto {
  firstName: string;
  lastName: string;
  dateOfBirth?: Date | null;
  bio?: string | null;
}

export class UpdatePublisherDto {
  name: string;
  address: string;
}

export class UpdateCategoryDto {
  name: string;
}
