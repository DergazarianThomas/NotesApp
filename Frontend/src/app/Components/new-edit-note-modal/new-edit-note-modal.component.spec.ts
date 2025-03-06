import { ComponentFixture, TestBed } from '@angular/core/testing';

import { NewEditNoteModalComponent } from './new-edit-note-modal.component';

describe('NewEditNoteModalComponent', () => {
  let component: NewEditNoteModalComponent;
  let fixture: ComponentFixture<NewEditNoteModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [NewEditNoteModalComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(NewEditNoteModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
