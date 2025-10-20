import { Component } from '@angular/core';
import { Employee } from '../../models/employee'; 
import { EmployeeService } from '../employee.service';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';

@Component({
  selector: 'employee-table',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './employee-table.component.html',
  styleUrl: './employee-table.component.css'
})
export class EmployeeTableComponent {

  employees: Employee[] = [];

  constructor(private employeeService: EmployeeService, private router: Router) {
    
  }

  loadEmployees() {
    this.employeeService.getEmployees().subscribe({
      next: (data) => {
        this.employees = data;
      },
      error: (error) => {
        console.error('Error fetching employees:', error);
      }
    });
  }

  ngOnChanges() {
    this.loadEmployees();
  }

  ngOnInit() {
    this.employeeService.getEmployees().subscribe((data) => {
      this.employees = data;
      console.log('Employees loaded:', this.employees);
    });
  }

  deleteEmployee(id: number) {
    this.employeeService.deleteEmployee(id).subscribe({
      next: () => {
        console.log('Employee deleted successfully');
        this.loadEmployees(); // Refresh the employee list
      },
      error: (error) => {
        console.error('Error deleting employee:', error);
      }
    });
  }

  editEmployee(id: number) {
    this.router.navigate(['/edit', id]);
  }
}
