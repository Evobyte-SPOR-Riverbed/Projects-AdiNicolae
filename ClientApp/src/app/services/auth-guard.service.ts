import { Injectable } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { CanActivate } from '@angular/router';
import { JwtHelperService } from '@auth0/angular-jwt';
import { AccessDialogComponent } from '../components/access-dialog/access-dialog.component';

@Injectable()
export class AuthGuardService implements CanActivate {
  constructor(private accessDialog: MatDialog, private jwtHelper: JwtHelperService) { }

  canActivate() {
    const token = localStorage.getItem("accessToken");
    if (token && !this.jwtHelper.isTokenExpired(token)) {
      return true;
    }
    // TODO: Tab control in order to open the Login tab.
    this.openAccessDialog();
    return false;
  }

  private openAccessDialog(): void {
    this.accessDialog.open(AccessDialogComponent, {
      width: '24%'
    });
  }
}
