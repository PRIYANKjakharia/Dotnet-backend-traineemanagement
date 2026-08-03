import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddTaskAssignment } from './add-task-assignment';

describe('AddTaskAssignment', () => {
  let component: AddTaskAssignment;
  let fixture: ComponentFixture<AddTaskAssignment>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AddTaskAssignment],
    }).compileComponents();

    fixture = TestBed.createComponent(AddTaskAssignment);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
