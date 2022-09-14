import { Component } from '@angular/core';
import { MatCheckboxChange } from '@angular/material/checkbox';
import { IPagedResponse } from '../../helpers/interfaces.module';
import { FiltersService, IGlass, IIngredient, IPreparationMethod } from '../../services/filters.service';

@Component({
  selector: 'app-filters-drawer',
  templateUrl: './filters-drawer.component.html',
  styleUrls: ['./filters-drawer.component.css']
})
export class FiltersDrawerComponent {
  glassesFilterResponse: IPagedResponse<IGlass> | undefined;
  ingredientsFilterResponse: IPagedResponse<IIngredient> | undefined;
  prepMethodsFilterResponse: IPagedResponse<IPreparationMethod> | undefined;
  glassFilters: IGlass[] = [];
  ingredientFilters: IGlass[] = [];
  prepMethodFilters: IGlass[] = [];

  constructor(public filterService: FiltersService) {
    this.filterService.getGlasses()
      .subscribe(pagedGlassesResponse => {
        this.glassesFilterResponse = pagedGlassesResponse;
        this.glassFilters = pagedGlassesResponse.data;
      });

    this.filterService.getIngredients()
      .subscribe(pagedIngredientsResponse => {
        this.ingredientsFilterResponse = pagedIngredientsResponse;
        this.ingredientFilters = pagedIngredientsResponse.data;
      });

    this.filterService.getPreparationMethods()
      .subscribe(pagedPrepMethodsResponse => {
        this.prepMethodsFilterResponse = pagedPrepMethodsResponse;
        this.prepMethodFilters = pagedPrepMethodsResponse.data;
      });
  }

  get notFoundGlasses(): boolean {
    return this.glassesFilterResponse != null && this.glassFilters.length == 0;
  }

  get notFoundIngredients(): boolean {
    return this.ingredientsFilterResponse != null && this.ingredientFilters.length == 0;
  }

  get notFoundPrepMethods(): boolean {
    return this.prepMethodsFilterResponse != null && this.prepMethodFilters.length == 0;
  }

  get canShowMoreGlasses(): boolean {
    return this.glassesFilterResponse != undefined && this.glassesFilterResponse.nextPage != null;
  }

  get canShowMoreIngredients(): boolean {
    return this.ingredientsFilterResponse != undefined && this.ingredientsFilterResponse.nextPage != null;
  }

  get canShowMorePrepMethods(): boolean {
    return this.prepMethodsFilterResponse != undefined && this.prepMethodsFilterResponse.nextPage != null;
  }

  onGlassChange(event: MatCheckboxChange) {
    if (event.checked) {
      this.filterService.applyGlass(event.source.value);
    }
    else {
      this.filterService.denyGlass(event.source.value);
    }
    // console.log(`Glass ID: ${event.source.value} - ${event.checked}`);
  }

  onIngredientChange(event: MatCheckboxChange) {
    if (event.checked) {
      this.filterService.applyIngredient(event.source.value);
    }
    else {
      this.filterService.denyIngredient(event.source.value);
    }
    // console.log(`Ingredient ID: ${event.source.value} - ${event.checked}`);
  }

  onPrepMethodChange(event: MatCheckboxChange) {
    if (event.checked) {
      this.filterService.applyPrepMethod(event.source.value);
    }
    else {
      this.filterService.denyPrepMethod(event.source.value);
    }
    // console.log(`Preparation Method ID: ${event.source.value} - ${event.checked}`);
  }

  showMoreGlasses(): void {
    this.filterService.getGlassesNextPage(this.glassesFilterResponse!)
      .subscribe(pagedGlassesResponse => {
        this.glassesFilterResponse = pagedGlassesResponse;
        this.glassFilters = this.glassFilters.concat(pagedGlassesResponse.data);
      });
  }

  showMoreIngredients(): void {
    this.filterService.getGlassesNextPage(this.ingredientsFilterResponse!)
      .subscribe(pagedIngredientsResponse => {
        this.ingredientsFilterResponse = pagedIngredientsResponse;
        this.ingredientFilters = this.ingredientFilters.concat(pagedIngredientsResponse.data);
      });
  }

  showMorePrepMethods(): void {
    this.filterService.getPreparationMethodsNextPage(this.prepMethodsFilterResponse!)
      .subscribe(pagedPrepMethodsResponse => {
        this.prepMethodsFilterResponse = pagedPrepMethodsResponse;
        this.prepMethodFilters = this.prepMethodFilters.concat(pagedPrepMethodsResponse.data);
      });
  }
}
