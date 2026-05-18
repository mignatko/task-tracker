import React, { useCallback, useEffect, useState } from "react";
import { TaskForm } from "./components/TaskForm";
import { TaskList } from "./components/TaskList";
import { SearchBar } from "./components/SearchBar";
import { StatsPanel } from "./components/StatsPanel";
import { TaskItem, CreateTaskRequest, getTasks, createTask, updateTask, deleteTask } from "./services/api";

function App() {
  const [tasks, setTasks] = useState<TaskItem[]>([]);
  const [filterStatus, setFilterStatus] = useState("");
  const [filterPriority, setFilterPriority] = useState("");
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");
  const [searchActive, setSearchActive] = useState(false);

  const loadTasks = useCallback(async () => {
    try {
      setLoading(true);
      const data = await getTasks(filterStatus || undefined, filterPriority || undefined);
      setTasks(data);
      setError("");
    } catch (err) {
      setError("Failed to load tasks");
    } finally {
      setLoading(false);
    }
  }, [filterStatus, filterPriority]);

  useEffect(() => {
    loadTasks();
  }, [loadTasks]);

  const handleCreate = async (request: CreateTaskRequest) => {
    try {
      await createTask(request);
      loadTasks();
    } catch {
      setError("Failed to create task");
    }
  };

  const handleStatusChange = async (id: number, status: string) => {
    try {
      await updateTask(id, { status });
      loadTasks();
    } catch {
      setError("Failed to update task");
    }
  };

  const handleDelete = async (id: number) => {
    try {
      await deleteTask(id);
      loadTasks();
    } catch {
      setError("Failed to delete task");
    }
  };

  return (
    <div style={{ maxWidth: 800, margin: "0 auto", padding: 24 }}>
      <h1>Task Tracker</h1>

      <StatsPanel />

      <SearchBar
        onResults={(results) => {
          setTasks(results);
          setSearchActive(true);
        }}
        onClear={() => {
          setSearchActive(false);
          loadTasks();
        }}
      />

      <TaskForm onSubmit={handleCreate} />

      <div style={{ display: "flex", gap: 12, marginBottom: 16, opacity: searchActive ? 0.5 : 1 }}>
        <select value={filterStatus} onChange={(e) => setFilterStatus(e.target.value)} disabled={searchActive} style={{ padding: 8 }}>
          <option value="">All Statuses</option>
          <option value="Todo">Todo</option>
          <option value="InProgress">In Progress</option>
          <option value="Done">Done</option>
        </select>
        <select value={filterPriority} onChange={(e) => setFilterPriority(e.target.value)} disabled={searchActive} style={{ padding: 8 }}>
          <option value="">All Priorities</option>
          <option value="Low">Low</option>
          <option value="Medium">Medium</option>
          <option value="High">High</option>
          <option value="Critical">Critical</option>
        </select>
      </div>

      {error && <p style={{ color: "red" }}>{error}</p>}
      {loading ? <p>Loading...</p> : <TaskList tasks={tasks} onStatusChange={handleStatusChange} onDelete={handleDelete} />}
    </div>
  );
}

export default App;
