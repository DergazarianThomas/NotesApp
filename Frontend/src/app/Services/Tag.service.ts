import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../enviroments/enviroment';
import { Tag, TagDTO, TagCreationDTO } from '../models/tag.model';

@Injectable({
  providedIn: 'root'
})
export class TagService {
  private apiUrl = `${environment.apiUrl}/api/tags`;

  constructor(private http: HttpClient) { }


  getTags(): Observable<TagDTO[]> {
    return this.http.get<TagDTO[]>(this.apiUrl);
  }


  getTag(id: number): Observable<Tag> {
    return this.http.get<Tag>(`${this.apiUrl}/${id}`);
  }


  createTag(tag: TagCreationDTO): Observable<Tag> {
    return this.http.post<Tag>(this.apiUrl, tag);
  }


  deleteTag(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}