import {
  IActivity,
  IApiErrorResponse,
  IApiResponse,
  ILoginData,
  ILoginResponseData,
  ISignupData,
  ITracking,
  IUser,
} from "./types";

async function errorHandlingMiddleware<T>(
  response: Response,
): Promise<IApiResponse<T> | IApiErrorResponse> {
  try {
    return await response.json();
  } catch (err) {
    console.error(err);
    return {
      isSuccess: false,
      message: "Something went wrong",
      errors: err instanceof Error ? err.message : "",
    };
  }
}

export const fetchActivities = async (
  userId: string,
): Promise<IApiResponse<IActivity[]> | IApiErrorResponse> => {
  return fetch(`/api/v1/activity/user/${userId}`, {
    method: "GET",
    headers: {
      "Content-Type": "application/json",
    },
  }).then(errorHandlingMiddleware<IActivity[]>);
};

export const fetchActivity = async (
  activityId: string,
): Promise<IApiResponse<IActivity> | IApiErrorResponse> => {
  return fetch(`/api/v1/activity/${activityId}`, {
    method: "GET",
    headers: {
      "Content-Type": "application/json",
    },
  }).then(errorHandlingMiddleware<IActivity>);
};

export const createActivity = async (
  name: string,
  userId: string,
): Promise<IApiResponse<IActivity> | IApiErrorResponse> => {
  return fetch("/api/v1/activity", {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify({ name, userId }),
  }).then(errorHandlingMiddleware<IActivity>);
};

export const updateActivityName = async (
  activity: IActivity,
): Promise<IApiResponse<IActivity> | IApiErrorResponse> => {
  return fetch(`/api/v1/activity/${activity.id}`, {
    method: "PUT",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify(activity),
  }).then(errorHandlingMiddleware<IActivity>);
};

export const deleteActivity = async (
  activityId: string,
): Promise<IApiResponse<string> | IApiErrorResponse> => {
  return fetch(`/api/v1/activity/${activityId}`, {
    method: "DELETE",
    headers: {
      "Content-Type": "application/json",
    },
  }).then(errorHandlingMiddleware<string>);
};

export const fetchTrackings = async (): Promise<
  IApiResponse<ITracking[]> | IApiErrorResponse
> => {
  return fetch("/api/v1/tracking", {
    method: "GET",
    headers: {
      "Content-Type": "application/json",
    },
  }).then(errorHandlingMiddleware<ITracking[]>);
};

export const startTracking = async (
  activityId: string,
): Promise<IApiResponse<ITracking> | IApiErrorResponse> => {
  return fetch("/api/v1/tracking", {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify({ activityId }),
  }).then(errorHandlingMiddleware<ITracking>);
};

export const updateTracking = async (
  tracking: ITracking,
): Promise<IApiResponse<ITracking> | IApiErrorResponse> => {
  return fetch(`/api/v1/tracking/${tracking.id}`, {
    method: "PUT",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify(tracking),
  }).then(errorHandlingMiddleware<ITracking>);
};

export const deleteTracking = async (
  trackingId: string,
): Promise<IApiResponse<string> | IApiErrorResponse> => {
  return fetch(`/api/v1/tracking/${trackingId}`, {
    method: "DELETE",
    headers: {
      "Content-Type": "application/json",
    },
  }).then(errorHandlingMiddleware<string>);
};

export const fetchUser = async (
  token: string,
): Promise<IApiResponse<IUser> | IApiErrorResponse> => {
  return fetch("/api/v1/authentication/user", {
    method: "GET",
    headers: {
      "Content-Type": "application/json",
      Authorization: `Bearer ${token}`,
    },
  }).then(errorHandlingMiddleware<IUser>);
};

export const loginUser = async (
  loginData: ILoginData,
): Promise<IApiResponse<ILoginResponseData> | IApiErrorResponse> => {
  return fetch("/api/v1/authentication/login", {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify(loginData),
  }).then(errorHandlingMiddleware<ILoginResponseData>);
};

export const signupUser = async (
  signupData: ISignupData,
): Promise<IApiResponse<string> | IApiErrorResponse> => {
  return fetch("/api/v1/authentication/register", {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify(signupData),
  }).then(errorHandlingMiddleware<string>);
};
