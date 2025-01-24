import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProcesoDespachoComponent } from './proceso-despacho.component';

describe('ProcesoDespachoComponent', () => {
  let component: ProcesoDespachoComponent;
  let fixture: ComponentFixture<ProcesoDespachoComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ProcesoDespachoComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ProcesoDespachoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
