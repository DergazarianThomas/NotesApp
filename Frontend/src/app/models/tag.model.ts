import { Note } from './note.model';

export interface Tag {
    id: number;
    name: string;
    notes?: Note[];
  }
  
  export interface TagDTO {
    id: number;
    name: string;
    noteCount: number;
  }
  
  export interface TagCreationDTO {
    name: string;
  }

