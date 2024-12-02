import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BookarooComponent } from './bookaroo.component';

describe('BookarooComponent', () => {
  let component: BookarooComponent;
  let fixture: ComponentFixture<BookarooComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [BookarooComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(BookarooComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
