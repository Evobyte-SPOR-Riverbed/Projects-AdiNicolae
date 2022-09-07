import { Component, Input } from '@angular/core';
import { AuthenticationService } from '../../services/authentication.service';

@Component({
  selector: 'app-cocktail-card',
  templateUrl: './cocktail-card.component.html',
  styleUrls: ['./cocktail-card.component.css']
})
export class CocktailCardComponent {
  @Input()
  cocktailName: string = "Template Cocktail";
  preparationMethodName: string = "Template Preparation Method";
  glassName: string = "Template Glass";
  cocktailImageUrl: string = "https://www.eatthis.com/wp-content/uploads/sites/4/2019/03/old-fashioned-cocktail.jpg";
  cocktailDescription: string = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Donec consequat, neque a efficitur suscipit, nisl tellus sollicitudin velit, ac dictum neque erat quis nisi. Fusce mollis quis sapien quis ultrices. Fusce dignissim elementum odio, a viverra turpis dapibus vel. Proin eleifend tellus a molestie euismod. Quisque viverra risus at lacus sagittis bibendum. Maecenas ac est id augue varius ornare quis a ante. Donec interdum et tortor in dapibus. Morbi sed nunc ipsum.";

  constructor(public authenticationService: AuthenticationService) { }
}
