import { Inject, Injectable } from '@angular/core';
import { Location } from '@angular/common';

@Injectable()
export class DrawerService {
  visible: boolean;

  constructor(@Inject(Location) private readonly location: Location) {
    const pathAfterDomainName = this.location.path();
    this.visible = pathAfterDomainName == "/";
  }

  get(): boolean {
    return this.visible;
  }

  hide() {
    this.visible = false;
  }

  show() {
    this.visible = true;
  }

  toggle() {
    this.visible = !this.visible;
  }
}
