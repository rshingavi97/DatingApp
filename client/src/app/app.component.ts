import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';


@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'client';
  users : any;
  constructor(private objHttp: HttpClient)
  {

  }
  ngOnInit():void{
    this.getUsers();
  }
  getUsers():void{
    this.objHttp.get('https://localhost:5001/api/users/')
    .subscribe(
              response=>{
                this.users = response;
              },
              error=>{
                console.log("Error generated at calling USERS API");
              });
  }
}
