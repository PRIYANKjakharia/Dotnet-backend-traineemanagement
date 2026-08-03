import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LearningTaskList } from './learning-task-list';

describe('LearningTaskList', () => {
  let component: LearningTaskList;
  let fixture: ComponentFixture<LearningTaskList>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [LearningTaskList],
    }).compileComponents();

    fixture = TestBed.createComponent(LearningTaskList);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
