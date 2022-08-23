import { Component, Inject } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { DrawerService } from '../../services/drawer.service';
import { AccessDialogComponent } from '../access-dialog/access-dialog.component';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent {
  constructor(public accessDialog: MatDialog, @Inject(DrawerService) public drawer: DrawerService) { }

  openAccessDialog(): void {
    const dialogRef = this.accessDialog.open(AccessDialogComponent, {
      width: '24%'
    });

    dialogRef.afterClosed().subscribe(() => {
      console.log('The dialog was closed');
    });
  }
}
