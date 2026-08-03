import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TaskAssignmentList } from './task-assignment-list';

describe('TaskAssignmentList', () => {
  let component: TaskAssignmentList;
  let fixture: ComponentFixture<TaskAssignmentList>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TaskAssignmentList],
    }).compileComponents();

    fixture = TestBed.createComponent(TaskAssignmentList);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
