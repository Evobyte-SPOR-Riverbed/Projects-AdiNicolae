import { Component, Inject } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MiscModule } from '../../helpers/misc.module';
import { AuthenticationService } from '../../services/authentication.service';
import { CountryService, ICountry } from '../../services/country.service';
import { DrawerService } from '../../services/drawer.service';
import { AccessDialogComponent } from '../access-dialog/access-dialog.component';
import { SettingsDialogComponent } from '../settings-dialog/settings-dialog.component';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent {
  userCountry: ICountry | undefined;
  constructor(public accessDialog: MatDialog, public authenticationService: AuthenticationService, private countryService: CountryService, public drawer: DrawerService,
    public settingsDialog: MatDialog, private snackBar: MatSnackBar) {
    if (authenticationService.isAuthenticated) {
      this.countryService.getCountryByAlphaCode(authenticationService.currentUserValue.countryAlpha2)
        .subscribe(countryData => {
          this.userCountry = countryData;
        })
    }
  }

  openAccessDialog(): void {
    if (this.authenticationService.isAuthenticated) {
      return;
    }

    const dialogRef = this.accessDialog.open(AccessDialogComponent, {
      width: '24%'
    });

    dialogRef.afterClosed().subscribe(() => {
      if (this.authenticationService.isAuthenticated) {
        this.countryService.getCountryByAlphaCode(this.authenticationService.currentUserValue.countryAlpha2)
          .subscribe(countryData => {
            this.userCountry = countryData;
          })
      }
    });
  }

  openSettingsDialog(): void {
    if (!this.authenticationService.isAuthenticated) {
      return;
    }

    const dialogRef = this.settingsDialog.open(SettingsDialogComponent, {
      width: '24%'
    });

    dialogRef.afterClosed().subscribe(() => {
      if (this.authenticationService.isAuthenticated) {
        this.countryService.getCountryByAlphaCode(this.authenticationService.currentUserValue.countryAlpha2)
          .subscribe(countryData => {
            this.userCountry = countryData;
          })
      }
    });
  }

  logout(): void {
    this.snackBar.open('You have successfully logged out.');
    this.authenticationService.logout();
  }

  get sexInitial(): string {
    return this.sexString[0];
  }

  get sexString(): string {
    return MiscModule.toSexString(this.authenticationService.currentUserValue.sex);
  }

  get age(): number {
    return MiscModule.calculateAge(this.authenticationService.currentUserValue.birthday);
  }
}
