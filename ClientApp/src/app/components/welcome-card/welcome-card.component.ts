import { Component } from '@angular/core';

@Component({
  selector: 'app-welcome-card',
  templateUrl: './welcome-card.component.html',
  styleUrls: ['./welcome-card.component.css']
})
export class WelcomeCardComponent {
  foreverClosedWelcomeCard: boolean;
  welcomeCardShown: boolean;
  constructor() {
    this.foreverClosedWelcomeCard = false;
    this.welcomeCardShown = !JSON.parse(localStorage.getItem("foreverClosedWelcomeCard") ?? "false");
  }

  closeWelcomeCard() {
    this.welcomeCardShown = false;
    if (this.foreverClosedWelcomeCard) {
      localStorage.setItem("foreverClosedWelcomeCard", JSON.stringify(this.foreverClosedWelcomeCard));
    }
  }
}
