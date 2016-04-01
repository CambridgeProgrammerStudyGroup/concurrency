; Fibonacci lazy-seq
(defn fib [a b]
  (lazy-seq
    (cons a
      (fib b (+ b a))
    )
  )
)

(take 10 (fib 1 1))
; user=> (1 1 2 3 5 8 13 21 34 55)


(ns sum.core)

; sum: naïve recursive version
(defn recursive-sum-naïve [numbers]
  (if (empty? numbers)
    0
    (+
      (first numbers)
      (recursive-sum-naïve (rest numbers))
    )
  )
)

(time (recursive-sum-naïve (range 1 10000000)))
; sum.core=> StackOverflowError


; sum: recursive with loop&recur
(defn recursive-sum [numbers]
  (loop [s 0 n numbers]
    (if (empty? n)
      s
      (recur (+ s (first n)) (rest n))
    )
  )
)

(time (recursive-sum (range 1 10000000)))
; "Elapsed time: 967.560663 msecs"
; sum.core=> 49999995000000


; sum: reduce with #()
(defn reduce-sum [numbers]
  (reduce #(+ %1 %2) numbers)
)

(time (reduce-sum (range 1 10000000)))
; "Elapsed time: 297.71603 msecs"
; sum.core=> 49999995000000


; su: parallel version
(ns sum.core (:require [clojure.core.reducers :as r]))

(defn parallel-sum [numbers] (r/fold + numbers))

(time (parallel-sum (range 1 10000000)))
; "Elapsed time: 293.932829 msecs"
; sum.core=> 49999995000000
