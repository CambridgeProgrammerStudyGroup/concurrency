; Day 2

(require '[clojure.core.reducers :as r])

(r/map (partial * 2) [1 2 3 4])

(reduce conj [] (r/map (partial * 2) [1 2 3 4]))

(into [] (r/map (partial * 2) [1 2 3 4]))

(into [] (r/map (partial + 1) (r/filter even? [1 2 3 4])))

; (defprotocol CollReduce
;   (coll-reduce [coll f] [coll f init]))

(ns reducers.core
  (:require [clojure.core.protocols :refer [CollReduce coll-reduce]]
            [clojure.core.reducers :refer [CollFold coll-fold]]))

(defn my-reduce
 ([f coll] (coll-reduce coll f))
 ([f init coll] (coll-reduce coll f init)))

(my-reduce + [1 2 3 4])

(defn make-reducer [reducible transformf]
  (reify
    CollReduce
    (coll-reduce [_ f1]
      (coll-reduce reducible (transformf f1) (f1)))
    (coll-reduce [_ f1 init]
      (coll-reduce reducible (transformf f1) init))))

(defn my-map [mapf reducible]
  (make-reducer reducible
    (fn [reducef]
      (fn [acc v]
        (reducef acc (mapf v))))))

(into [] (my-map (partial * 2) [1 2 3 4]))

(def my-reducible
  (reify
    CollReduce
    (coll-reduce [_ f1]
      (reduce f1 [1 2 3]))
    (coll-reduce [_ f1 init]
      (reduce f1 init [1 2 3]))))

(coll-reduce my-reducible conj [])


;; Exercises

; flatten/mapcat/filter implemented using reduce

(defn flatten-by-reduce [x]
  (reduce
    (fn [acc v]
      (if (sequential? v)
        (if (empty? v)
          acc
          (concat acc (flatten-by-reduce v)))
        (concat acc [v])))
    [] x))


(flatten-by-reduce [[1] [2 3 [[4 5]]] 6])


(defn mapcat-by-reduce [mapf x]
  (reduce
    (fn [acc v]
      (concat acc (mapf v)))
    [] x))

(mapcat-by-reduce (fn [x] (cons x [x])) [1 2 3])


(defn filter-by-reduce [filterf x]
  (reduce
    (fn [acc v]
      (if (filterf v)
        (concat acc [v])
        acc))
    [] x))

(filter-by-reduce (fn [x] (odd? x)) [1 2 3])



; flatten/mapcat/filter reducer versions

(defn my-flatten [reducible]
  (make-reducer reducible
    (fn [reducef]
      (fn [acc v]
        (if (sequential? v)
          (coll-reduce (my-flatten v) reducef acc)
          (reducef acc v))))))

(into [] (my-flatten [[1] [2 3 [[4 5]]] 6]))


(defn my-mapcat [mapf reducible]
  (make-reducer reducible
    (fn [reducef]
      (fn [acc v]
        (coll-reduce (mapf v) reducef acc)))))

(into [] (my-mapcat (fn [x] (cons x [x])) [1 2 3]))


(defn my-filter [filterf reducible]
  (make-reducer reducible
    (fn [reducef]
      (fn [acc v]
        (if (filterf v)
          (reducef acc v)
          acc)))))

(into [] (my-filter (fn [x] (odd? x)) [1 2 3]))

