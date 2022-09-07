import { Component, Inject } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { AuthenticationService } from '../../services/authentication.service';
import { DrawerService } from '../../services/drawer.service';
import { AccessDialogComponent } from '../access-dialog/access-dialog.component';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent {
  constructor(public accessDialog: MatDialog, public authenticationService: AuthenticationService, @Inject(DrawerService) public drawer: DrawerService, private snackBar: MatSnackBar) { }

  openAccessDialog(): void {
    if (this.authenticationService.isAuthenticated) {
      return;
    }

    this.accessDialog.open(AccessDialogComponent, {
      width: '24%'
    });
  }

  logout(): void {
    this.snackBar.open('You have successfully logged out.');
    this.authenticationService.logout();
  }
}
