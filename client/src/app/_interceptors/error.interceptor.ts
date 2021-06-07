import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { NavigationExtras, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { catchError, defaultIfEmpty } from 'rxjs/operators'; 

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {

  constructor(private router:Router, private toastr:ToastrService) {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    return next.handle(request).pipe(
      catchError(
        error=>{
          if(error)
          {
            switch (error.status)
            {
              case 400:
                if( error.error.errors)  //in case Array of errors inside response
                {
                  /* put all the error messages into this array and then send it to component so that it could display such error messages specifically. Nevertheless, it is not good idea to show array of errors through ToastService thats why storing into an array*/
                  const modalStateErrors = []; 
                  for ( const key in error.error.errors)
                  {
                    if(error.error.errors[key])
                      modalStateErrors.push(error.error.errors[key]);
                  }
                  throw modalStateErrors;//send it to component
                    /*Ideal scenario for this array of errors is Username and Password validation.
    If both could be wrong then more than one error messages would be stored into Errors of response 400.
    so component could show all such errors to the user so that user could take the action.*/
                }
                else
                {
                  this.toastr.error(error.statusText, error.status);
                  //since just one error message has received so just show it through Toaster service.
                }
                break; //end of case 400

              case 401:
                this.toastr.error(error.statusText, error.status);
                break;
              case 404: //Not found then redirect to the NOT-FOUND page by using Router.
                this.router.navigateByUrl('/not-found');
                break;
              case 500:
                const navigateExtras: NavigationExtras = {state:{error: error.error}};
                this.router.navigateByUrl('/server-error',navigateExtras);
                /* navigateExtras sending the details about error to the SERVER-ERROR PAGE*/
                break;
              default:
                this.toastr.error('Something unexpected went wrong');
                console.log(error);
                break;
            }

          }
          return throwError(error); 
          /*if we dont catch the error above then pass on such error,where the request generated, 
          by calling return*/
        }//end of error

      )// end of catchError
    );//end of pipe
  }//end of intercept
}



