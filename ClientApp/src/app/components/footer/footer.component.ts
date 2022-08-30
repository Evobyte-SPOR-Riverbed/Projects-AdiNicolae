import { Component } from '@angular/core';

interface Tile {
  color: string;
  icon: string;
  title: string;
  url: string;
  target: string;
  links: Link[];
}

interface Link {
  title: string;
  url: string;
  target: string;
}

@Component({
  selector: 'app-footer',
  templateUrl: './footer.component.html',
  styleUrls: ['./footer.component.css']
})

export class FooterComponent {
  tiles: Tile[] = [
    {
      title: 'Drinktionary', url: '/', icon: 'logo', target: '_self', color: 'primary', links: [
        { title: 'Random Cocktail', url: '/random-cocktail', target: '_self' },
        { title: 'Github Repository', url: 'https://github.com/Evobyte-SPOR-Riverbed/Projects-AdiNicolae/tree/FinalProject', target: '_blank' },
        { title: 'Contact Us!', url: 'mailto:nicolae.adrian.m@gmail.com', target: '_blank' }
      ]
    },
    {
      title: 'Angular', url: 'https://angular.io/', icon: 'angular', target: '_blank', color: 'primary', links: [
        { title: 'Features', url: 'https://angular.io/features', target: '_blank' },
        { title: 'Docs', url: 'https://angular.io/docs', target: '_blank' },
        { title: 'Resources', url: 'https://angular.io/resources', target: '_blank' },
        { title: 'Events', url: 'https://angular.io/events', target: '_blank' },
        { title: 'Blog', url: 'https://blog.angular.io', target: '_blank' },
      ]
    },
    {
      title: 'Angular Material', url: 'https://material.angular.io', icon: 'angular_material', target: '_blank', color: 'primary', links: [
        { title: 'Components', url: 'https://material.angular.io/components', target: '_blank' },
        { title: 'CDK', url: 'https://material.angular.io/cdk', target: '_blank' },
        { title: 'Guides', url: 'https://material.angular.io/guides', target: '_blank' }
      ]
    }
  ]
}
