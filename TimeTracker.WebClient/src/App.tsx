import { BrowserRouter, Route, Routes } from "react-router";
import "./App.css";
import { ListView } from "./components/ListView";
import { NavBar } from "./components/NavBar";
import { LoginForm } from "./components/LoginForm";
import { SignUpForm } from "./components/SignUpForm";
import { PrivateRoute } from "./components/PrivateRoute";
import { AuthenticationProvider } from "./components/AuthenticationProvider";
import { GlobalStateProvider } from "./components/GlobalStateProvider";

function App() {
  return (
    <>
      <BrowserRouter>
        <AuthenticationProvider>
          <NavBar />
          <Routes>
            <Route path="/login" element={<LoginForm />} />
            <Route path="/signup" element={<SignUpForm />} />
            <Route element={<PrivateRoute />}>
              <Route
                path="/"
                element={
                  <GlobalStateProvider>
                    <ListView />
                  </GlobalStateProvider>
                }
              />
            </Route>
          </Routes>
        </AuthenticationProvider>
      </BrowserRouter>
    </>
  );
}

export default App;
