import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, RouterOutlet} from '@angular/router';
import { InnerStuffComponent } from "./inner-stuff/inner-stuff.component";
import { routeComponent } from './route/route.component';
import { errorComponent } from './error/error.component';
import { rulesComponent } from './rules/rules.component';
import { UserService } from './user.service';

@Component({
    selector: 'app-root',
    standalone: true,
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.css'],
    imports: [CommonModule, RouterModule, InnerStuffComponent, RouterOutlet, routeComponent, errorComponent, rulesComponent],
    providers: [UserService]
})

export class AppComponent implements OnInit {
  constructor(private userService: UserService) { }
  ngOnInit()
  {
    this.userService.getWords().subscribe(words => {
      console.log(words);
    });
  }
}
