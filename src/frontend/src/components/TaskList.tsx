import React from "react";
import { TaskItem } from "../services/api";

interface TaskListProps {
  tasks: TaskItem[];
  onStatusChange: (id: number, status: string) => void;
  onDelete: (id: number) => void;
}

const statusColors: Record<string, string> = {
  Todo: "#6b7280",
  InProgress: "#f59e0b",
  Done: "#10b981",
};

const priorityColors: Record<string, string> = {
  Low: "#9ca3af",
  Medium: "#3b82f6",
  High: "#f97316",
  Critical: "#ef4444",
};

export function TaskList({ tasks, onStatusChange, onDelete }: TaskListProps) {
  if (tasks.length === 0) {
    return <p style={{ color: "#999", textAlign: "center" }}>No tasks yet. Add one above!</p>;
  }

  return (
    <div style={{ display: "flex", flexDirection: "column", gap: 12 }}>
      {tasks.map((task) => (
        <div key={task.id} style={{ padding: 16, border: "1px solid #e5e7eb", borderRadius: 8, display: "flex", justifyContent: "space-between", alignItems: "center" }}>
          <div>
            <div style={{ display: "flex", gap: 8, alignItems: "center", marginBottom: 4 }}>
              <strong>{task.title}</strong>
              <span style={{ fontSize: 12, padding: "2px 6px", borderRadius: 4, background: priorityColors[task.priority] || "#ccc", color: "white" }}>
                {task.priority}
              </span>
              <span style={{ fontSize: 12, padding: "2px 6px", borderRadius: 4, background: statusColors[task.status] || "#ccc", color: "white" }}>
                {task.status}
              </span>
            </div>
            {task.description && <p style={{ margin: 0, color: "#666", fontSize: 14 }}>{task.description}</p>}
            <div style={{ fontSize: 12, color: "#999", marginTop: 4 }}>
              {task.assignedTo && <span>Assigned to: {task.assignedTo} · </span>}
              {task.dueDate && <span>Due: {new Date(task.dueDate).toLocaleDateString()} · </span>}
              Created: {new Date(task.createdAt).toLocaleDateString()}
            </div>
          </div>
          <div style={{ display: "flex", gap: 8 }}>
            <select
              value={task.status}
              onChange={(e) => onStatusChange(task.id, e.target.value)}
              style={{ padding: 4, fontSize: 12 }}
            >
              <option value="Todo">Todo</option>
              <option value="InProgress">In Progress</option>
              <option value="Done">Done</option>
            </select>
            <button
              onClick={() => onDelete(task.id)}
              style={{ padding: "4px 8px", background: "#ef4444", color: "white", border: "none", borderRadius: 4, cursor: "pointer", fontSize: 12 }}
            >
              Delete
            </button>
          </div>
        </div>
      ))}
    </div>
  );
}
