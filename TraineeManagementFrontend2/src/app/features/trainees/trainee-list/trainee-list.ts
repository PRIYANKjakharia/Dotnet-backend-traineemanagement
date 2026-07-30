import { Component, OnInit, signal } from '@angular/core';
import { TraineeService } from '../trainee.service';
import { TraineeQuery } from '../../../shared/models/trainee-query';
import { Trainee } from '../../../shared/models/trainee';
import { FormsModule } from '@angular/forms';
@Component({
  selector: 'app-trainee-list',
  imports: [FormsModule],
  templateUrl: './trainee-list.html',
  styleUrl: './trainee-list.css'
})
export class TraineeList implements OnInit {
  
  constructor(private traineeService: TraineeService) {}
  trainees: Trainee[] = [];
  loading = signal<boolean>(false);
  errorMessage = '';
  query: TraineeQuery = {
    pageNumber: 1,
    pageSize: 10,
    search: '',
    status: ''
  };
  totalRecords = 0;
  ngOnInit(): void {
    // this.loading.set(true);
    this.loadTrainees();
    // this.traineeService.getAll(this.query).subscribe({
      //   next: (response) => {
        // this.zone.run(()=>{
          
        // this.trainees = response.data;
        // console.log("loading before ",this.loading);
        // this.loading.set(false);
        // console.log("loading after ",this.loading);
        // console.log("here");
        // this.cdr.detectChanges();
        
        // queueMicrotask(()=>{
          //   this.loading = false;
        // });
      // });
    // },
    // error: (error) => {
            
    //   if (error.status === 401) {
    //     this.errorMessage = 'Unauthorized';
    //   } else if (error.status === 403) {
      //     this.errorMessage = 'Forbidden';
    //   } else {
      //     this.errorMessage = 'Something went wrong.';
    //   }
      
      //   this.loading.set(false);
      // }
    // });
  }
    
  search():void{
    this.query.pageNumber = 1;
    this.loadTrainees();
  }
  filterByStatus():void{
    this.query.pageNumber = 1;
    this.loadTrainees();
  }
  
  changePageSize() {
    this.query.pageNumber = 1;
    this.loadTrainees();
  }

  loadTrainees(): void {
    this.loading.set(true);
    this.trainees = [];
    this.errorMessage = '';
    this.traineeService.getAll(this.query).subscribe({
      next: (response) => {
        this.trainees = response.data;
        this.totalRecords = response.totalRecords;
        this.loading.set(false);
      },
      error: (error) => {
        
        if (error.status === 401) {
          this.errorMessage = 'Unauthorized';
        } else if (error.status === 403) {
          this.errorMessage = 'Forbidden';
        } else {
          this.errorMessage = 'Something went wrong.';
        }
        this.loading.set(false);
      }
    });
    
  }

  previousPage(): void{
    if(this.query.pageNumber > 1){
      this.query.pageNumber--;
      this.loadTrainees();
    }
  }
  nextPage(): void{
    if(this.query.pageNumber * this.query.pageSize < this.totalRecords){
      this.query.pageNumber++;
      this.loadTrainees();
    }
  }
}