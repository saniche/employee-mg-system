import { Component } from '@angular/core';
import { Employee } from '../../models/employee'; 
import { EmployeeService } from '../employee.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'employee-table',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './employee-table.component.html',
  styleUrl: './employee-table.component.css'
})
export class EmployeeTableComponent {

  employees: Employee[] = [];

  constructor(private employeeService: EmployeeService) {
    
  }

  loadEmployees() {
    this.employeeService.getEmployees().subscribe(
      (data) => {
        this.employees = data;
      },
      (error) => {
        console.error('Error fetching employees:', error);
      }
    );
  }

  ngOnInit() {
    this.employeeService.getEmployees().subscribe((data) => {
      this.employees = data;
      console.log('Employees loaded:', this.employees);
    });
  }
}
