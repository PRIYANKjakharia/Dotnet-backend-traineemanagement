export interface PagedResponse<T>{
    
    pageNumber:number;
    pageSize:number;
    totalRecords:number;
    data: T[];
}