import { defineConfig, loadEnv } from "vite";
import react from "@vitejs/plugin-react-swc";

// https://vite.dev/config/
export default defineConfig(({ mode }) => {
  const env = loadEnv(mode, process.cwd());

  const API_URL = `${env.VITE_API_URL ?? "http://localhost:5000"}`;

  return {
    plugins: [react()],
    server: {
      proxy: {
        "/api": {
          target: API_URL,
          changeOrigin: true,
        },
      },
    },
  };
});