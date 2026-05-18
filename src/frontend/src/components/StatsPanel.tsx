import React, { useEffect, useState } from "react";
import { getStats, TaskStats } from "../services/api";

export function StatsPanel() {
  const [stats, setStats] = useState<TaskStats | null>(null);

  useEffect(() => {
    getStats().then(setStats);
  }, []);

  if (!stats) return null;

  return (
    <div style={{ display: "flex", gap: 16, marginBottom: 24, padding: 16, background: "#f3f4f6", borderRadius: 8 }}>
      <div style={{ textAlign: "center" }}>
        <div style={{ fontSize: 24, fontWeight: "bold" }}>{stats.total}</div>
        <div style={{ fontSize: 12, color: "#666" }}>Total</div>
      </div>
      <div style={{ textAlign: "center" }}>
        <div style={{ fontSize: 24, fontWeight: "bold", color: "#6b7280" }}>{stats.todo}</div>
        <div style={{ fontSize: 12, color: "#666" }}>Todo</div>
      </div>
      <div style={{ textAlign: "center" }}>
        <div style={{ fontSize: 24, fontWeight: "bold", color: "#f59e0b" }}>{stats.inProgress}</div>
        <div style={{ fontSize: 12, color: "#666" }}>In Progress</div>
      </div>
      <div style={{ textAlign: "center" }}>
        <div style={{ fontSize: 24, fontWeight: "bold", color: "#10b981" }}>{stats.done}</div>
        <div style={{ fontSize: 12, color: "#666" }}>Done</div>
      </div>
      <div style={{ textAlign: "center" }}>
        <div style={{ fontSize: 24, fontWeight: "bold", color: "#ef4444" }}>{stats.overdue}</div>
        <div style={{ fontSize: 12, color: "#666" }}>Overdue</div>
      </div>
      <div style={{ textAlign: "center" }}>
        <div style={{ fontSize: 24, fontWeight: "bold" }}>{stats.avgCompletionDays.toFixed(1)}</div>
        <div style={{ fontSize: 12, color: "#666" }}>Avg Days</div>
      </div>
    </div>
  );
}
