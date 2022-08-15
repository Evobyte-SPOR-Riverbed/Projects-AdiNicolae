import { Component, Inject } from '@angular/core';
import { DrawerService } from '../../services/drawer.service';

@Component({
  selector: 'app-new-home',
  templateUrl: './new-home.component.html',
  styleUrls: ['./new-home.component.css']
})
export class NewHomeComponent {
  welcomeCardShown: boolean;
  sampleCocktails: number[];
  constructor(@Inject(DrawerService) public drawer: DrawerService) {
    this.welcomeCardShown = true;
    this.sampleCocktails = Array(50).fill(0);
  }
}
