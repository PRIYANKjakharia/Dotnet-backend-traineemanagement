import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddSubmission } from './add-submission';

describe('AddSubmission', () => {
  let component: AddSubmission;
  let fixture: ComponentFixture<AddSubmission>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AddSubmission],
    }).compileComponents();

    fixture = TestBed.createComponent(AddSubmission);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
