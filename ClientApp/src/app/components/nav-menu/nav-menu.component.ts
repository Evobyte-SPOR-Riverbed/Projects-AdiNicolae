import { Component, Inject } from '@angular/core';
import { DrawerService } from '../../services/drawer.service';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent {
  constructor(@Inject(DrawerService) public drawer: DrawerService) { }
}
