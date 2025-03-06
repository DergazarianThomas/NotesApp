import { Injectable } from '@angular/core';
import { HttpClient, HttpParams, HttpResponse } from '@angular/common/http';
import { Observable, map } from 'rxjs';
import { environment } from '../../enviroments/enviroment';
import { 
  Note, 
  NoteDTO, 
  NoteCreationDTO, 
  NoteUpdateDTO, 
  NoteStatusUpdateDTO,
  NotesFilter,
  FilteredNotesResult,
} from '../models/note.model';

@Injectable({
  providedIn: 'root'
})
export class NoteService {
  private apiUrl = `${environment.apiUrl}/api/Notes`;

  constructor(private http: HttpClient) { }


  getNotes(): Observable<NoteDTO[]> {
    return this.http.get<NoteDTO[]>(this.apiUrl);
  }


  getFilteredNotes(filter: NotesFilter): Observable<FilteredNotesResult> {
    let params = new HttpParams()
      .set('page', filter.page.toString())
      .set('pageSize', filter.pageSize.toString());
    
    if (filter.status !== undefined) {
      params = params.set('status', filter.status.toString());
    }
    
    if (filter.searchTerm) {
      params = params.set('searchTerm', filter.searchTerm);
    }
    
    if (filter.tagIds && filter.tagIds.length > 0) {
      filter.tagIds.forEach(id => {
        params = params.append('tagIds', id.toString());
      });
    }

    return this.http.get<NoteDTO[]>(`${this.apiUrl}/filtered`, {
      params,
      observe: 'response'
    }).pipe(
      map((response: HttpResponse<NoteDTO[]>) => {
        const totalCount = parseInt(response.headers.get('X-Total-Count') || '0', 10);
        const totalPages = parseInt(response.headers.get('X-Total-Pages') || '0', 10);
        
        return {
          notes: response.body || [],
          totalCount,
          totalPages,
          currentPage: filter.page,
          pageSize: filter.pageSize
        };
      })
    );
  }


  getNote(id: number): Observable<NoteDTO> {
    return this.http.get<NoteDTO>(`${this.apiUrl}/${id}`);
  }


  createNote(note: NoteCreationDTO): Observable<Note> {
    return this.http.post<Note>(this.apiUrl, note);
  }


  updateNote(id: number, note: NoteUpdateDTO): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, note);
  }


  updateNoteStatus(id: number, statusUpdate: NoteStatusUpdateDTO): Observable<void> {
    return this.http.patch<void>(`${this.apiUrl}/${id}/status`, statusUpdate);
  }


  deleteNote(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}