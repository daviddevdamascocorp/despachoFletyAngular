import { Component, ContentChild, Input, TemplateRef } from '@angular/core';
import { Router, RouteConfigLoadStart, RouteConfigLoadEnd } from '@angular/router';
import { Observable, tap } from 'rxjs';
import { LoadingService } from '../loadser/loading.service';


@Component({
  selector: 'app-loader',
  templateUrl: './loader.component.html',
  styleUrl: './loader.component.css'
})
export class LoaderComponent {
  loading$ !: Observable<boolean>

  @Input()
  detectRouteTransitions = false;

  @ContentChild("loading")
  customLoadingIndicator: TemplateRef<any> | null = null;

  /**
   *
   */
  constructor(
    private loadingService:LoadingService,
    private router:Router,
    
  ) {
    
    this.loading$ = this.loadingService.loading$;
  }
  ngOnInit(): void {
   if(this.detectRouteTransitions){
    this.router.events.pipe(
      tap((event)=>{
        if(event instanceof RouteConfigLoadStart){
          this.loadingService.loadingOn();
        }else if(event instanceof RouteConfigLoadEnd){
          this.loadingService.loadingOff()
        }
      })
    ).subscribe()
   }
  }

}
