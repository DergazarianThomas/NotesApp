import { Component, HostListener, OnInit } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NoteService } from '../app/Services/Note.service';
import { TagService } from '../app/Services/Tag.service';
import { NewEditNoteModalComponent } from './Components/new-edit-note-modal/new-edit-note-modal.component';
import { 
  NoteDTO, 
  NotesFilter,
  FilteredNotesResult
} from '../app/models/note.model';
import { TagDTO, TagCreationDTO  } from '../app/models/tag.model';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [FormsModule, CommonModule, HttpClientModule, NewEditNoteModalComponent],
  providers: [NoteService, TagService],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit {


  title = 'FrontendNotas';
  activeView: 'true' | 'false' = 'true';
  searchText: string = '';

  tagForm: FormGroup;
  newTagName: string = '';

  notes: NoteDTO[] = [];
  tags: TagDTO[] = [];
  isLoading = false;
  errorMessage = '';

  filter: NotesFilter = {
    page: 1,
    pageSize: 10,
    status: true,
    tagIds: [],
  };
  resultsData: FilteredNotesResult | null = null;

  isDropdownOpen = false;
  isNoteModalOpen = false;
  selectedNote: NoteDTO | null = null;

  constructor( private noteService: NoteService, private tagService: TagService, private fb: FormBuilder){
    this.tagForm = this.fb.group({
      name: ['', [Validators.required, Validators.minLength(2)]]
    });
  }

 
  ngOnInit(): void {
      this.loadTags();
      this.filteredNotes();
  }

  loadTags(): void {
    this.tagService.getTags()
      .subscribe({
        next: (data) => {
          this.tags = data;
        },
        error: (error) => {
          this.errorMessage = 'Error loading tags: ' + error.message;
        }
      });
  }

  filteredNotes(): void {
    this.isLoading = true;
    this.noteService.getFilteredNotes(this.filter)
      .subscribe({
        next: (result) => {
          this.resultsData = result;
          this.notes = result.notes;
          this.isLoading = false;
        },
        error: (error) => {
          this.errorMessage = 'Error loading notes: ' + error.message;
          this.isLoading = false;
        }
      });
  }
  
  onTagChange(event: Event): void {
    const checkbox = event.target as HTMLInputElement;
    const tagId = parseInt(checkbox.value, 10);

    if (checkbox.checked) {
      this.filter.tagIds?.push(tagId);
    } else {
      this.filter.tagIds = this.filter.tagIds?.filter(id => id !== tagId);
    }

    this.applyFilter();
  }


  setView(view: 'true' | 'false'): void {
    this.activeView = view;
    this.filter.status = view === 'true';
    this.applyFilter();
  }

  applyFilter(): void {
    this.filter.page = 1;
    this.filteredNotes();
  }

  toggleDropdown(): void {
    this.isDropdownOpen = !this.isDropdownOpen;
  }

  @HostListener('document:click', ['$event'])
  onDocumentClick(event: Event): void {
    const target = event.target as HTMLElement;
    if (!target.closest('#options-menu') && !target.closest('#options-menu-button')) {
      this.isDropdownOpen = false;
    }
  }

  deleteNote(id: number): void {
    if (confirm('Are you sure you want to delete this note?')) {
      this.noteService.deleteNote(id)
        .subscribe({
          next: () => {
            this.filteredNotes();
          },
          error: (error) => {
            this.errorMessage = 'Error deleting note: ' + error.message;
          }
        });
    }
  }

  updateNoteStatus(id: number, status: boolean): void {
    if (confirm('Are you sure you want to change this note status?')) {
      this.noteService.updateNoteStatus(id, { status })
        .subscribe({
          next: () => {
            this.filteredNotes();
          },
          error: (error) => {
            this.errorMessage = 'Error updating note status: ' + error.message;
          }
        });
    }
  }

  createTag(): void {
    if (this.newTagName.trim()) {
      const newTag: TagCreationDTO = {
        name: this.newTagName.trim()
      };

      this.tagService.createTag(newTag)
        .subscribe({
          next: () => {
            this.loadTags();
            this.newTagName = '';
          },
          error: (error) => {
            this.errorMessage = 'Error creating tag: ' + error.message;
          }
        });
    }
  }

  deleteTag(id: number): void {
    if (confirm('Are you sure you want to delete this tag?')) {
      this.tagService.deleteTag(id)
        .subscribe({
          next: () => {
            this.loadTags();
          },
          error: (error) => {
            this.errorMessage = 'Error deleting tag: ' + error.message;
          }
        });
    }
  } 

  addNote(): void {
    this.selectedNote = null;
    this.isNoteModalOpen = true;
    console.log(this.isNoteModalOpen);
  }

  editNote(note: NoteDTO): void {
    this.selectedNote = note;
    this.isNoteModalOpen = true;
  }

  closeNoteModal(): void {
    this.isNoteModalOpen = false;
    this.selectedNote = null;
  }

  onNoteSaved(): void {
    this.filteredNotes();
  }
  
  manageTags(): void {
    this.toggleDropdown();
  }
  
}


