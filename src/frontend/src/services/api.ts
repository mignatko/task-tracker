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

export interface Comment {
  id: number;
  taskItemId: number;
  author: string;
  content: string;
  createdAt: string;
}

export interface TaskStats {
  total: number;
  todo: number;
  inProgress: number;
  done: number;
  overdue: number;
  avgCompletionDays: number;
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

export async function searchTasks(query: string): Promise<TaskItem[]> {
  const response = await fetch(`${API_BASE}/tasks/search?q=${query}`);
  return response.json();
}

export async function getComments(taskId: number): Promise<Comment[]> {
  const response = await fetch(`${API_BASE}/tasks/${taskId}/comments`);
  return response.json();
}

export async function addComment(taskId: number, author: string, content: string): Promise<Comment> {
  const response = await fetch(`${API_BASE}/tasks/${taskId}/comments`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify({ author, content }),
  });
  return response.json();
}

export async function deleteComment(taskId: number, commentId: number): Promise<void> {
  await fetch(`${API_BASE}/tasks/${taskId}/comments/${commentId}`, { method: "DELETE" });
}

export async function getStats(): Promise<TaskStats> {
  const response = await fetch(`${API_BASE}/stats`);
  return response.json();
}

export async function bulkUpdateTasks(updates: { id: number; status: string }[]): Promise<void> {
  await fetch(`${API_BASE}/tasks/bulk-update`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(updates),
  });
}

export async function exportTasksCsv(): Promise<string> {
  const response = await fetch(`${API_BASE}/tasks/export`);
  return response.text();
}
