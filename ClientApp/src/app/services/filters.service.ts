import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { IPagedResponse } from '../helpers/interfaces.module';

export interface IGlass {
  id: string;
  name: string;
  description: string;
  cocktailNumber: number;
}

export interface IIngredient {
  id: string;
  name: string;
  description: string;
  cocktailNumber: number;
}

export interface IPreparationMethod {
  id: string;
  name: string;
  description: string;
  cocktailNumber: number;
}

@Injectable({
  providedIn: 'root'
})
export class FiltersService {
  private httpOptions = {
    headers: new HttpHeaders({
      'Accept': 'application/json',
      'Content-Type': 'application/json'
    })
  }

  private appliedGlassFilters: string[] = [];
  private appliedIngredientFilters: string[] = [];
  private appliedPrepMethodFilters: string[] = [];

  constructor(@Inject('BASE_URL') private baseUrl: string, private httpClient: HttpClient) { }

  getGlasses(pageNumber?: number, pageSize?: number, searchFilter?: string): Observable<IPagedResponse<IGlass>> {
    let queryNeeded = pageNumber != undefined || pageSize != undefined || searchFilter != undefined;
    let pageNumberQuery = pageNumber != undefined ? `pageNumber=${pageNumber}&` : '';
    let pageSizeQuery = pageNumber != undefined ? `pageSize=${pageNumber}&` : '';
    let searchFilterQuery = pageNumber != undefined ? `searchFilter=${searchFilter}` : '';
    let query = queryNeeded ? `?${pageNumberQuery}${pageSizeQuery}${searchFilterQuery}` : '';
    return this.httpClient.get<IPagedResponse<IGlass>>(`${this.baseUrl}api/filters/glasses${query}`, this.httpOptions);
  }

  getIngredients(pageNumber?: number, pageSize?: number, searchFilter?: string): Observable<IPagedResponse<IIngredient>> {
    let queryNeeded = pageNumber != undefined || pageSize != undefined || searchFilter != undefined;
    let pageNumberQuery = pageNumber != undefined ? `pageNumber=${pageNumber}&` : '';
    let pageSizeQuery = pageNumber != undefined ? `pageSize=${pageNumber}&` : '';
    let searchFilterQuery = pageNumber != undefined ? `searchFilter=${searchFilter}` : '';
    let query = queryNeeded ? `?${pageNumberQuery}${pageSizeQuery}${searchFilterQuery}` : '';
    return this.httpClient.get<IPagedResponse<IIngredient>>(`${this.baseUrl}api/filters/ingredients${query}`, this.httpOptions);
  }

  getPreparationMethods(pageNumber?: number, pageSize?: number, searchFilter?: string): Observable<IPagedResponse<IPreparationMethod>> {
    let queryNeeded = pageNumber != undefined || pageSize != undefined || searchFilter != undefined;
    let pageNumberQuery = pageNumber != undefined ? `pageNumber=${pageNumber}&` : '';
    let pageSizeQuery = pageNumber != undefined ? `pageSize=${pageNumber}&` : '';
    let searchFilterQuery = pageNumber != undefined ? `searchFilter=${searchFilter}` : '';
    let query = queryNeeded ? `?${pageNumberQuery}${pageSizeQuery}${searchFilterQuery}` : '';
    return this.httpClient.get<IPagedResponse<IPreparationMethod>>(`${this.baseUrl}api/filters/preparationmethods${query}`, this.httpOptions);
  }

  getGlassesNextPage(lastPage: IPagedResponse<IGlass>): Observable<IPagedResponse<IGlass>> {
    return this.httpClient.get<IPagedResponse<IGlass>>(lastPage.nextPage!, this.httpOptions);
  }

  getIngredientsNextPage(lastPage: IPagedResponse<IIngredient>): Observable<IPagedResponse<IIngredient>> {
    return this.httpClient.get<IPagedResponse<IIngredient>>(lastPage.nextPage!, this.httpOptions);
  }

  getPreparationMethodsNextPage(lastPage: IPagedResponse<IPreparationMethod>): Observable<IPagedResponse<IPreparationMethod>> {
    return this.httpClient.get<IPagedResponse<IPreparationMethod>>(lastPage.nextPage!, this.httpOptions);
  }

  applyGlass(glassId: string): void {
    this.appliedGlassFilters.push(glassId);
    console.log(this.appliedGlassFilters);
  }

  applyIngredient(ingredientId: string): void {
    this.appliedIngredientFilters.push(ingredientId);
    console.log(this.appliedIngredientFilters);
  }

  applyPrepMethod(prepMethodId: string): void {
    this.appliedPrepMethodFilters.push(prepMethodId);
    console.log(this.appliedPrepMethodFilters);
  }

  denyGlass(glassId: string): void {
    const index = this.appliedGlassFilters.indexOf(glassId, 0);
    if (index > -1) {
      this.appliedGlassFilters.splice(index, 1);
      console.log(this.appliedGlassFilters);
    }
  }

  denyIngredient(ingredientId: string): void {
    const index = this.appliedIngredientFilters.indexOf(ingredientId, 0);
    if (index > -1) {
      this.appliedIngredientFilters.splice(index, 1);
      console.log(this.appliedIngredientFilters);
    }
  }

  denyPrepMethod(prepMethodId: string): void {
    const index = this.appliedPrepMethodFilters.indexOf(prepMethodId, 0);
    if (index > -1) {
      this.appliedPrepMethodFilters.splice(index, 1);
      console.log(this.appliedPrepMethodFilters);
    }
  }

  isGlassApplied(glassId: string): boolean {
    const index = this.appliedGlassFilters.indexOf(glassId, 0);
    return index > -1;
  }

  isIngredientApplied(ingredientId: string): boolean {
    const index = this.appliedIngredientFilters.indexOf(ingredientId, 0);
    return index > -1;
  }

  isPrepMethodApplied(prepMethodId: string): boolean {
    const index = this.appliedPrepMethodFilters.indexOf(prepMethodId, 0);
    return index > -1;
  }

  get appliedGlassIds(): string[] {
    return this.appliedGlassFilters;
  }

  get appliedIngredientIds(): string[] {
    return this.appliedIngredientFilters;
  }

  get appliedPrepMethodIds(): string[] {
    return this.appliedPrepMethodFilters;
  }
}
