(ns concurrency.core
  (:gen-class)
  (:require [clojure.core.reducers :as r]))

; Recursive version of sum
(defn recursive-sum [numbers]
  (if (empty? numbers)
    0
    (+ (first numbers) (recursive-sum (rest numbers)))))

; Exercise 1
; Iterative version of sum
(defn recursive-sum-loop [numbers]
  (loop [sum 0 iteration 0]
    (if (= iteration (count numbers))
      (print "Done!")
      (recur (+ (nth numbers iteration) sum) (inc iteration)))))

(defn reduce-sum [numbers]
  (reduce (fn [acc x] (+ acc x)) 0 numbers))

; Exercise 2
(defn reduce-sum2 [numbers]
  (reduce #(+ %1 %2) 0 numbers))

(defn sum [numbers]
  (reduce + numbers))

(defn parallel-sum [numbers]
  (r/fold sum-simple numbers))

(defn word-frequencies [words]
  (reduce
    (fn [counts word] (assoc counts word (inc (get counts word 0))))
     {} words))

(defn get-words [text] (re-seq #"\w+" text))

(defn count-words-sequential [pages]
 (frequencies (mapcat get-words pages)))

(defn count-words-parallel [pages]
  (reduce (partial merge-with +)
    (pmap #(frequencies (get-words %)) pages)))

(defn -main
  "I don't do a whole lot ... yet."
  [& args]

  ; Exercises
  (println (reduce-sum2 [1 2 3]))
  (println (recursive-sum-loop [1 2 3]))

  (def pages ["one potato two potato three potato four" "five potato six potato seven potato more"])
  (println (pmap #(frequencies (get-words %)) pages))
)
