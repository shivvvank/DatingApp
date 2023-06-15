import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  //props 
  title:string = 'Hello!!';
  users:any;

  //constructor
  constructor(private http:HttpClient){
    
  }


}
