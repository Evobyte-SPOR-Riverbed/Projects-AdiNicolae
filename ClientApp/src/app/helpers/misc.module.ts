import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DrinkerType, SexType } from './enums.module';

@NgModule({
  declarations: [],
  imports: [
    CommonModule
  ]
})
export class MiscModule {
  static fromSexString(sexString: string): SexType {
    return ['male', 'female', 'intersex', 'other'].indexOf(sexString.toLowerCase());
  }

  static toSexString(sexType: SexType): string {
    return ['Male', 'Female', 'Intersex', 'Other'][sexType];
  }

  static fromDrinkerTypeString(drinkerTypeString: string): DrinkerType {
    return ['socialdrinker', 'conformitydrinker', 'enhancementdrinker', 'copingdrinker', 'none'].indexOf(drinkerTypeString.toLowerCase());
  }

  static toDrinkerTypeString(drinkerType: DrinkerType): string {
    return ['Social drinker', 'Conformity drinker', 'Enhancement drinker', 'Coping drinker', 'None'][drinkerType];
  }

  static calculateAge(birthDate: Date): number {
    let timeDiff = Math.abs(Date.now() - birthDate.getTime());
    return Math.floor((timeDiff / (1000 * 3600 * 24)) / 365.25);
  }
}
