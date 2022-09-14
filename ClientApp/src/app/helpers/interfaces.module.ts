export interface IPagedResponse<T> {
  totalRecords: number;
  nextPage: string | null;
  previousPage: string | null;
  data: T[];
}
