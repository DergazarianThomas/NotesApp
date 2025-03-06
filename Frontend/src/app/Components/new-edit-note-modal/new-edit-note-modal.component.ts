import { Component, EventEmitter, Input, OnChanges, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NoteDTO, NoteCreationDTO, NoteUpdateDTO } from '../../models/note.model';
import { TagDTO } from '../../models/tag.model';
import { NoteService } from '../../Services/Note.service';

@Component({
  selector: 'app-new-edit-note-modal',
  standalone: true,
  imports: [CommonModule, FormsModule, ReactiveFormsModule],
  templateUrl: './new-edit-note-modal.component.html',
  styleUrl: './new-edit-note-modal.component.css'
})

export class NewEditNoteModalComponent implements OnChanges {
  @Input() isOpen = false;
  @Input() note: NoteDTO | null = null;

  @Input() availableTags: TagDTO[] = [];
  
  @Output() close = new EventEmitter<void>();
  @Output() saved = new EventEmitter<void>();
  
  noteForm: FormGroup;
  selectedTagIds: number[] = [];
  
  constructor(
    private fb: FormBuilder,
    private noteService: NoteService
  ) {
    this.noteForm = this.fb.group({
      title: ['', Validators.required],
      content: [''],
      status: [true]
    });
  }
  
  ngOnChanges(): void {
    if (this.note) {
      this.noteForm.patchValue({
        title: this.note.title,
        content: this.note.content,
        status: this.note.status
      });
      console.log(this.note);
      this.selectedTagIds = this.note.tagIds;
      console.log(this.selectedTagIds);
    } else {
      this.resetForm();
    }
  }
  
  resetForm(): void {
    this.noteForm.reset({
      title: '',
      content: '',
      status: true
    });
    this.selectedTagIds = [];
  }
  
  isTagSelected(tagId: number): boolean {
    return this.selectedTagIds.includes(tagId);
  }
  
  onTagChange(event: Event): void {
    const checkbox = event.target as HTMLInputElement;
    const tagId = parseInt(checkbox.value, 10);
    
    if (checkbox.checked) {
      if (!this.selectedTagIds.includes(tagId)) {
        this.selectedTagIds.push(tagId);
      }
    } else {
      this.selectedTagIds = this.selectedTagIds.filter(id => id !== tagId);
    }
  }
  
  onSubmit(): void {
    if (this.noteForm.valid) {
      const formValue = this.noteForm.value;
      
      if (this.note) {
        const updateDto: NoteUpdateDTO = {
          title: formValue.title,
          content: formValue.content,
          status: formValue.status,
          tagIds: this.selectedTagIds
        };
        
        this.noteService.updateNote(this.note.id, updateDto).subscribe({
          next: () => {
            this.saved.emit();
            this.close.emit();
          },
          error: (error) => {
            console.error('Error updating note:', error);
          }
        });
      } else {
        const creationDto: NoteCreationDTO = {
          title: formValue.title,
          content: formValue.content,
          status: formValue.status,
          tagIds: this.selectedTagIds
        };
        
        this.noteService.createNote(creationDto).subscribe({
          next: () => {
            this.saved.emit();
            this.resetForm();
            this.close.emit();
          },
          error: (error) => {
            console.error('Error creating note:', error);
          }
        });
      }
    }
  }
    closeModal(): void {
    this.close.emit();
  }

}
