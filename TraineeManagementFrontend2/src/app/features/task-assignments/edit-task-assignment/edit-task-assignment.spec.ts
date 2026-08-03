import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EditTaskAssignment } from './edit-task-assignment';

describe('EditTaskAssignment', () => {
  let component: EditTaskAssignment;
  let fixture: ComponentFixture<EditTaskAssignment>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [EditTaskAssignment],
    }).compileComponents();

    fixture = TestBed.createComponent(EditTaskAssignment);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
