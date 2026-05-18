const API_BASE = "http://localhost:5000/api";

export interface TaskItem {
  id: number;
  title: string;
  description?: string;
  priority: string;
  status: string;
  createdAt: string;
  dueDate?: string;
  assignedTo?: string;
}

export interface CreateTaskRequest {
  title: string;
  description?: string;
  priority: string;
  dueDate?: string;
  assignedTo?: string;
}

export interface UpdateTaskRequest {
  title?: string;
  description?: string;
  priority?: string;
  status?: string;
  dueDate?: string;
  assignedTo?: string;
}

export async function getTasks(status?: string, priority?: string): Promise<TaskItem[]> {
  const params = new URLSearchParams();
  if (status) params.append("status", status);
  if (priority) params.append("priority", priority);

  const url = params.toString() ? `${API_BASE}/tasks?${params}` : `${API_BASE}/tasks`;
  const response = await fetch(url);
  return response.json();
}

export async function getTask(id: number): Promise<TaskItem> {
  const response = await fetch(`${API_BASE}/tasks/${id}`);
  return response.json();
}

export async function createTask(request: CreateTaskRequest): Promise<TaskItem> {
  const response = await fetch(`${API_BASE}/tasks`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(request),
  });
  return response.json();
}

export async function updateTask(id: number, request: UpdateTaskRequest): Promise<TaskItem> {
  const response = await fetch(`${API_BASE}/tasks/${id}`, {
    method: "PUT",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(request),
  });
  return response.json();
}

export async function deleteTask(id: number): Promise<void> {
  await fetch(`${API_BASE}/tasks/${id}`, { method: "DELETE" });
}
