import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DespachoComprobanteComponent } from './despacho-comprobante.component';

describe('DespachoComprobanteComponent', () => {
  let component: DespachoComprobanteComponent;
  let fixture: ComponentFixture<DespachoComprobanteComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [DespachoComprobanteComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DespachoComprobanteComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
