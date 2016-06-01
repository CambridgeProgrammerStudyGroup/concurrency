(def sum (future (+ 1 2 3 4 5)))

(deref sum)

(let [a (future (+ 1 2))
    b (future (+ 3 4))]
  (+ @a @b))


(def meaning-of-life (promise))

(future (println "The meaning of life is:" @meaning-of-life))

(deliver meaning-of-life 42)
