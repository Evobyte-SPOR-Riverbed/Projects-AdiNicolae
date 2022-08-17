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
    this.welcomeCardShown = !JSON.parse(localStorage.getItem("neverWelcome") ?? "false");
  }

  closeWelcomeCard() {
    this.welcomeCardShown = false;
    if (this.foreverClosedWelcomeCard) {
      localStorage.setItem("neverWelcome", JSON.stringify(this.foreverClosedWelcomeCard));
    }
  }
}
