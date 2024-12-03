import { useState } from "react";
import { signupUser } from "../lib/api";
import { FormButton, TextButton } from "./Buttons";
import { useNavigate } from "react-router";
import { useAuthentication } from "../context/AuthenticationContext";

export const SignUpForm = () => {
  const [firstName, setFirstName] = useState("");
  const [lastName, setLastName] = useState("");
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [confirmPassword, setConfirmPassword] = useState("");
  const auth = useAuthentication();
  const navigate = useNavigate();

  const signup = async (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();
    if (
      !auth ||
      email === "" ||
      password === "" ||
      confirmPassword === "" ||
      firstName === "" ||
      lastName === ""
    ) {
      alert("Please provide valid values");
      return;
    }

    const result = await signupUser({
      data: {
        firstName,
        lastName,
        email,
        password,
        confirmPassword,
      },
    });

    if (result) {
      await auth.loginAction({ data: { email, password } });
    }

    // TODO: show error
  };

  return (
    <div className="-mt-16 flex h-screen">
      <div className="m-auto w-[400px]">
        <form
          onSubmit={signup}
          className="p-8 flex flex-col gap-4 bg-surface0 rounded-lg shadow-xl"
        >
          <input
            className="bg-surface0 border-b border-text focus:outline-none"
            onChange={(e) => setFirstName(e.target.value)}
            type="text"
            placeholder="First Name"
            id="first-name"
            value={firstName}
          />
          <input
            className="bg-surface0 border-b border-text focus:outline-none"
            onChange={(e) => setLastName(e.target.value)}
            type="text"
            placeholder="Last Name"
            id="last-name"
            value={lastName}
          />
          <input
            className="bg-surface0 border-b border-text focus:outline-none"
            onChange={(e) => setEmail(e.target.value)}
            type="email"
            placeholder="Email"
            id="email"
            value={email}
          />
          <input
            className="bg-surface0 border-b border-text focus:outline-none"
            onChange={(e) => setPassword(e.target.value)}
            type="password"
            placeholder="Password"
            id="password"
            value={password}
          />
          <input
            className="bg-surface0 border-b border-text focus:outline-none"
            onChange={(e) => setConfirmPassword(e.target.value)}
            type="password"
            placeholder="Confirm Password"
            id="confirm-password"
            value={confirmPassword}
          />
          <FormButton>Sign Up</FormButton>
          <TextButton onClick={() => navigate("/login")}>Login</TextButton>
        </form>
      </div>
    </div>
  );
};
