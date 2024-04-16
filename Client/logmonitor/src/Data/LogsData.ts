import { signal } from "@preact/signals-react";
import { logModels } from "../Models/Models";

export default {};

export const APIServerURL = signal<string>(import.meta.env.VITE_BASE_URL)
export const LogInfos = signal<Array<logModels>>([]);
export const postsData = signal<Array<any>>([]);