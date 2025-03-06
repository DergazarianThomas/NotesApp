import { Tag, TagDTO } from './tag.model';

export interface Note {
  id: number;
  title: string;
  content: string;
  status: boolean;
  tags?: Tag[];
}

export interface NoteDTO {
  id: number;
  title: string;
  content: string;
  status: boolean;
  tagIds: number[];
}

export interface NoteCreationDTO {
  title: string;
  content: string;
  status: boolean;
  tagIds: number[];
}

export interface NoteUpdateDTO {
  title: string;
  content: string;
  status: boolean;
  tagIds: number[];
}

export interface NoteStatusUpdateDTO {
  status: boolean;
}

export interface FilteredNotesResult {
  notes: NoteDTO[];
  totalCount: number;
  totalPages: number;
  currentPage: number;
  pageSize: number;
}

export interface NotesFilter {
  status?: boolean;
  searchTerm?: string;
  tagIds?: number[];
  page: number;
  pageSize: number;
}